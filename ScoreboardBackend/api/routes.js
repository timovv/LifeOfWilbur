module.exports = function (app) {
  const controller = require('./controller');

  app.route('/scores')
    .post(controller.submitScore);
};