﻿'use strict';
var help = '';
var gulp = require('gulp'),
	watch = require('gulp-watch'),
	sourceForTasks = require('./gulptasks/sourceForTasks'),
	lazyRequireTask = function (taskName, path, options) {
		options = options || {};
		options.taskName = taskName;
		gulp.task(taskName, function (callback) {
			var task = require(path).call(this, options);
			return task(callback);
		});
	};
	
for (var key in sourceForTasks) {
	help += '- gulp ' + key + '\n';
	lazyRequireTask(key, sourceForTasks[key].taskPath, sourceForTasks[key]);
}

gulp.task('build', gulp.series(
	gulp.parallel(gulp.series('copyPlatform', 'overrideLess', 'copyApp', 'copyAssemblies'), 'concatJs')
));

gulp.task('fullWatch', function () {
	watch(sourceForTasks.copyPlatform.src, gulp.series('copyPlatform', 'overrideLess'));
	watch(sourceForTasks.copyApp.src, gulp.series('copyApp'));
	watch(sourceForTasks.copyAssemblies.src, gulp.series('copyAssemblies'));
	watch(sourceForTasks.overrideLess.srcForWatch, gulp.series('overrideLess'));
	watch(sourceForTasks.concatJs.src, gulp.series('concatJs'));
});

gulp.task('default', function (cb) {
	console.log('####Task is not defined!\n' +
		'####Use any of defined tasks:\n' +
		help +
		'- gulp build\n' +
		'- gulp example'
	);
	cb();
});
