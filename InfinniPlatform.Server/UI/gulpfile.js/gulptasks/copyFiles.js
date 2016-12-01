'use strict';

var gulp = require('gulp'),
			$ = require('gulp-load-plugins')();

module.exports = function(options) {
	return function() {
		return gulp.src(options.src, {base: options.base})
			.pipe(gulp.dest(options.dest));
	};
};
