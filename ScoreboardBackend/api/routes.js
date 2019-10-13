module.exports = function(app) {
  var controller = require('./controller');

  app.route('/scores') // '/tasks/:taskId'
    .get(controller.listAll)
    .post(controller.submitScore)
    .delete(controller.removeAll);

  app.route('/scores/top10')
    .get(controller.listTopTen);
};