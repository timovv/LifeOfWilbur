const mongoose = require('mongoose');
const Score = mongoose.model('Score');

exports.submitScore = function (req, res) {
  const newScore = new Score(req.body);
  newScore.save(function (err, score) {

    const lt = Score.find({ "time": { $lte: score.time }, "_id": { $ne: score._id } })
      .sort("-time -attempts -timeswaps")
      .limit(5)
      .lean()
      .exec();

    const gt = Score.find({ "time": { $gte: score.time }, "_id": { $ne: score._id } })
      .sort("time attempts timeswaps")
      .limit(5)
      .lean()
      .exec();

    // Maybe swap to estimate - probably wont matter for our small db though
    const rank = Score.countDocuments({ "time": { $lte: score.time }, "attempts": { $lte: score.attempts }, "timeswaps": { $lte: score.timeswaps } })
      .exec();

    const top = Score.find({})
      .sort("time attempts timeswaps")
      .limit(10)
      .lean()
      .exec();

    Promise.all([rank, top, lt, gt]).then(function (results) {
      const info = {};
      const rank = results[0];

      info.id = score._id;

      info.top = results[1];
      addRankings(1, info.top);

      info.list = results[2].reverse();
      info.list.push(score.toObject());
      info.list = info.list.concat(results[3]);
      addRankings(rank - results[2].length + 1, info.list);

      res.json(info);
    });
  });
};

function addRankings(startingRank, list) {
  for (const score of list) {
    score.rank = startingRank++;
  }
}
