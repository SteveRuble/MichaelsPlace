var appRoot = 'src/';
var outputRoot = 'app/dist/';
var exportSourceRoot = 'app/';
var exportSrvRoot = 'export/';

module.exports = {
  root: appRoot,
  source: appRoot + '**/*.js',
  html: appRoot + '**/*.html',
  css: 'content/*.css',//appRoot + '**/*.css',
  style: 'styles/**/*.css',
  output: outputRoot,
  exportSourceRoot: exportSourceRoot,
  exportSrv: exportSrvRoot,
  doc: './doc',
  e2eSpecsSrc: 'test/e2e/src/**/*.js',
  e2eSpecsDist: 'test/e2e/dist/'
};
