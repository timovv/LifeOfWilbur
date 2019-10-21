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