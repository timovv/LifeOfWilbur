/**
 * Maps endpoints to functions
 */
module.exports = function (app) {
  const controller = require('./controller');

  app.route('/scores')
    .post(controller.submitScore);

  app.route('/scores/graph/:field')
    .get(controller.getGraphData);
};