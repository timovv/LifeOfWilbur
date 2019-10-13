var express = require('express'),
  app = express(),
  port = process.env.PORT || 3000,
  mongoose = require('mongoose'),
  Score = require('./api/model'),
  bodyParser = require('body-parser');
  
mongoose.Promise = global.Promise;
mongoose.connect('mongodb://localhost:27017/ScoreDB', {useNewUrlParser: true, useUnifiedTopology: true}); 

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

var routes = require('./api/routes');
routes(app);

app.listen(port);

//Debug
Score.deleteMany({}).then(function(){
for(var i = 0; i < 10; i+=2){
  new Score({ name: '0'+i, time: '0'+i, attempts: 0, timeswaps: 0 }).save();
}
for(var i = 10; i < 100; i+=2){
  new Score({ name: ''+i, time: ''+i, attempts: 0, timeswaps: 0 }).save();
}
});