var mongoose = require('mongoose');
var Score = mongoose.model('Score');

exports.submitScore = function(req, res) {
  var newScore = new Score(req.body);
  newScore.save(function(err, score) {
    
    var lt = Score.find({"time" : { $lte : score.time}, "_id" : { $ne : score._id}})
      .sort("-time -attempts -timeswaps")
      .limit(5)
      .exec();

    var gt = Score.find({"time" : { $gte : score.time}, "_id" : { $ne : score._id}})
      .sort("time attempts timeswaps")
      .limit(5)
      .exec();

    var rank = Score.find({"time" : { $lte : score.time}, "attempts" : { $lte : score.attempts}, "timeswaps" : { $lte : score.timeswaps}})
      .exec();

    Promise.all([lt, gt, rank]).then(function(results){
      var info = {};
      info.lt = results[0].reverse();
      info.gt = results[1];
      info.rank = results[2].length;
      res.json(info);
    });
  });
};

exports.listAll = function(req, res) {
  Score.find({}, function(err, scores) {
    res.json(scores);
  });
};

exports.removeAll = function(req, res) {
  Score.deleteMany({}, function(err, scores) {
    res.send("DELETED");
  });
};

exports.listTopTen = function(req, res) {
  Score.find({})
      .sort("time attempts timeswaps")
      .limit(10)
      .exec(function(err, scores) {
        res.json(scores);
      });
};