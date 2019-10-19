/**
 * Express server setup
 */
const express = require('express'),
  app = express(),
  port = process.env.PORT || 3000,
  mongoose = require('mongoose'),
  Score = require('./api/model'),
  bodyParser = require('body-parser');

mongoose.Promise = global.Promise;
mongoose.connect('mongodb://localhost:27017/ScoreDB', { useNewUrlParser: true, useUnifiedTopology: true });

app.use(bodyParser.urlencoded({ extended: true })); // For HTTP form encoding
app.use(bodyParser.json());

const routes = require('./api/routes');
routes(app);

app.listen(port);



//Debug - remove
Score.deleteMany({}).then(function () {
  for (var i = 0; i < 100; i++)
    new Score({ name: '62', time: randn_bm() * 600000, attempts: randn_bm() * 50, timeswaps: randn_bm() * 50 }).save();
});

// Retrived from https://stackoverflow.com/questions/25582882
function randn_bm() {
  var u = 0, v = 0;
  while (u === 0) u = Math.random(); //Converting [0,1) to (0,1)
  while (v === 0) v = Math.random();
  let num = Math.sqrt(-2.0 * Math.log(u)) * Math.cos(2.0 * Math.PI * v);
  num = num / 10.0 + 0.5; // Translate to 0 -> 1
  if (num > 1 || num < 0) return randn_bm(); // resample between 0 and 1
  return num;
}