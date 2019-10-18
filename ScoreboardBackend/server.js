const express = require('express'),
  app = express(),
  port = process.env.PORT || 3000,
  mongoose = require('mongoose'),
  Score = require('./api/model'),
  bodyParser = require('body-parser');

mongoose.Promise = global.Promise;
mongoose.connect('mongodb://localhost:27017/ScoreDB', { useNewUrlParser: true, useUnifiedTopology: true });

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

const routes = require('./api/routes');
routes(app);

app.listen(port);

//Debug
Score.deleteMany({}).then(function () {
  for (var i = 0; i < 1300; i++) {
    new Score({ name: '62', time: i * 600, attempts: i % 30, timeswaps: i % 40 }).save();
    if (i > 300) new Score({ name: '62', time: i * 600, attempts: i % 55, timeswaps: i % 55 }).save();
    if (i > 600) new Score({ name: '62', time: i * 600, attempts: i % 55, timeswaps: i % 55 }).save();
    if (i > 900) new Score({ name: '62', time: i * 600, attempts: i % 55, timeswaps: i % 55 }).save();
    if (i > 1200) new Score({ name: '62', time: i * 600, attempts: i % 55, timeswaps: i % 55 }).save();
    if (i > 1300) new Score({ name: '62', time: i * 600, attempts: i % 55, timeswaps: i % 55 }).save();

  }
});