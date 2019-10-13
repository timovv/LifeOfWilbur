var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var ScoreSchema = new Schema({
  name: {
    type: String,
    required: true
  },
  time: {
    type: String,
    required: true
  },
  attempts: {
    type: Number,
    min: 0,
    required: true
  },
  timeswaps: {
    type: Number,
    min: 0,
    required: true
  }
}, { versionKey: false });

module.exports = mongoose.model('Score', ScoreSchema);