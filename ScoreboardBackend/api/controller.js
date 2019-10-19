/**
 * Endpoint handlers
 */

const mongoose = require('mongoose');
const Score = mongoose.model('Score');

/**
 * Field boundaries for bucketing. Must be linear / equally sized.
 */
const fieldBoundaries = {
  "time": [0, 60, 120, 180, 240, 300, 360, 420, 480, 540, 600, 660],
  "attempts": [0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55],
  "timeswaps": [0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55]
}

/**
 * Handles POSTing of new scores
 * Add score to db and returns:
 *  - Top 10 Scores
 *  - Scores +/- 5 ranks of the new score
 */
exports.submitScore = function (req, res) {
  const newScore = new Score(req.body);

  // Check required fields
  const check = newScore.toObject();
  if (!("name" in check && "time" in check && "attempts" in check && "timeswaps" in check)) {
    res.sendStatus(400);
    return;
  }

  newScore.save(function (err, score) {
    // Maybe swap to estimate - probably wont matter for our small-ish db though
    const rank = Score.countDocuments({ "time": { $lte: score.time } })
      .exec();

    // Gets top ten entries
    const top = Score.find({})
      .sort("time attempts timeswaps")
      .limit(10)
      .lean()
      .exec();

    // 5 scores below
    const lt = Score.find({ "time": { $lte: score.time }, "_id": { $ne: score._id } })
      .sort("-time -attempts -timeswaps")
      .limit(5)
      .lean()
      .exec()
      .then(function (ltRes) {
        // To exclude duplicate 5 below results, occurs on ties
        const ids = ltRes.map(a => a._id);
        ids.push(score._id);

        // 5 scores above
        const gt = Score.find({ "time": { $gte: score.time }, "_id": { $nin: ids } })
          .sort("time attempts timeswaps")
          .limit(5)
          .lean()
          .exec();

        Promise.all([rank, top, gt]).then(function (results) {
          const info = {};
          const rank = results[0];

          info.id = score._id;

          info.top = results[1];
          addRankings(1, info.top);

          info.near = ltRes.reverse();
          info.near.push(score.toObject());
          info.near = info.near.concat(results[2]);

          // One final sort incase times are tied.
          info.near.sort(function (a, b) {
            return a.time - b.time || a.attempts - b.attempts || a.timeswaps - b.timeswaps;
          });

          addRankings(rank - ltRes.length + 1, info.near);

          res.json(info);
        });
      });
  });
};

/**
 * Adds ranks to each element in the list starting from the given rank. 
 */
function addRankings(startingRank, list) {
  for (const score of list) {
    score.rank = startingRank++;
  }
}

/**
 * GETS data for the specified statistic. Results  
 */
exports.getGraphData = function (req, res) {
  if (!(req.params.field in fieldBoundaries)) {
    res.sendStatus(404);
    return;
  }
  const boundaries = fieldBoundaries[req.params.field];

  const count = Score.countDocuments()
    .exec();

  const graph = Score.aggregate([
    {
      $bucket: {
        groupBy: "$" + req.params.field,
        boundaries: boundaries,
        default: "default",
        output: {
          "count": { $sum: 1 }
        }
      }
    }
  ])
    .exec();

  Promise.all([count, graph]).then(function (results) {
    const count = results[0];

    // Converts [{id:string, count:int}] to {id:count}
    const buckets = {};
    for (const bucket of results[1]) {
      buckets[bucket._id] = bucket.count;
    }

    const output = {};
    const bars = []
    output.bars = bars;
    output.max = boundaries[boundaries.length - 1];

    for (var i = 0; i < boundaries.length - 1; i++) {
      const bar = {};
      bar.range = [boundaries[i], boundaries[i + 1]];

      // DB results doesn't include empty buckets, manually set counts to 0
      if (boundaries[i] in buckets) {
        bar.count = buckets[boundaries[i]];
        bar.percentage = bar.count / count;
      } else {
        bar.count = 0;
        bar.percentage = 0;
      }
      bars.push(bar);
    }

    // Adds out of bound counts to the max bucket
    if ("default" in buckets) {
      const finalBar = bars[bars.length - 1]
      finalBar.count += buckets["default"];
      finalBar.percentage = finalBar.count / count;
    }
    res.json(output);
  });
}
