const mongoose = require('mongoose');
const Score = mongoose.model('Score');

const fieldBoundaries = {
  "time": [0, 60000, 120000, 180000, 240000, 300000, 360000, 420000, 480000, 540000, 600000, 660000],
  "attempts": [0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55],
  "timeswaps": [0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55]
}

exports.submitScore = function (req, res) {
  const newScore = new Score(req.body);
  newScore.save(function (err, score) {

    // Maybe swap to estimate - probably wont matter for our small db though
    const rank = Score.countDocuments({ "time": { $lte: score.time } })
      .exec();

    const top = Score.find({})
      .sort("time attempts timeswaps")
      .limit(10)
      .lean()
      .exec();

    const lt = Score.find({ "time": { $lte: score.time }, "_id": { $ne: score._id } })
      .sort("-time -attempts -timeswaps")
      .limit(5)
      .lean()
      .exec()
      .then(function (ltRes) {
        const ids = ltRes.map(a => a._id);
        ids.push(score._id);

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

          info.near.sort(function (a, b) {
            return a.time - b.time || a.attempts - b.attempts || a.timeswaps - b.timeswaps;
          });

          addRankings(rank - ltRes.length + 1, info.near);

          res.json(info);
        });
      });
  });
};

function addRankings(startingRank, list) {
  for (const score of list) {
    score.rank = startingRank++;
  }
}

exports.getGraphData = function (req, res) {
  if (!(req.params.field in fieldBoundaries)) {
    res.sendStatus(404);
    return;
  }
  const boundaries = fieldBoundaries[req.params.field];

  const count = Score.countDocuments().exec();

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

      if (boundaries[i] in buckets) {
        bar.count = buckets[boundaries[i]];
        bar.percentage = bar.count / count;
      } else {
        bar.count = 0;
        bar.percentage = 0;
      }
      bars.push(bar);
    }

    if ("default" in buckets) {
      const finalBar = bars[bars.length - 1]
      finalBar.count += buckets["default"];
      finalBar.percentage = finalBar.count / count;
    }
    res.json(output);
  });
}
