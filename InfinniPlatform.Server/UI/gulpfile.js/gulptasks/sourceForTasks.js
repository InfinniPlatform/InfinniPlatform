'use strict';
// Необходимо указать путь до платфомы в bower_components
var infinniUIpath = './bower_components/infinni-ui-v2/';

// Путь из infinniUIpath к прикладным стилям
var fromInfinniToNewStylesPath = '/../../styles/';

// Путь до папки-результата
var projectRootFolder = '../content/www/';
var solutionAssembliesFolder = '../../Assemblies/content';
// куда собирать платформу?
var projectFolderForPlatform = '../content/www/compiled/platform/';
// куда собирать прикладную часть?
var projectFolderForExtensions = '../content/www/compiled/js/';


// Платформенные перменные (не рекомендуется менять)
var platformOutputFolder = '/out/';
var stylesFile = '/app/styles/main.less';

var jsFiles = ['./js/**/*.js'];
var templateFiles = ["./js/**/*.tpl.html"];

var sourceForTasks = {
	cleanFolder: {
		src: projectFolderForPlatform,
		taskPath: "./gulptasks/cleanFolder"
	},
	overrideLess: {
		src: infinniUIpath + stylesFile,
		changedVariables: {
			"pl-override-platform-variables-path": '../..' + fromInfinniToNewStylesPath + 'platform-variables.less',
			"pl-override-bootstrap-variables-path": '../..' + fromInfinniToNewStylesPath + 'bootstrap-variables.less',
			"pl-bootstrap-theme-path": '../..' + fromInfinniToNewStylesPath + 'bootstrap-theme.less',
			"pl-extension-path": '../..' + fromInfinniToNewStylesPath + 'extensions.less'
		},
		srcForWatch: "./styles/",
		finalName: "main.css",
		dest: projectFolderForPlatform + "css/",
		taskPath: "./gulptasks/overrideLess"
	},
	copyPlatform: {
		src: [infinniUIpath + platformOutputFolder + '**/*.*', '!' + infinniUIpath + platformOutputFolder + 'unitTest.js'],
		dest: projectFolderForPlatform,
		taskPath: "./gulptasks/copyFiles"
	},
	copyApp: {
		src: ['./img/'+ '**/*.*', './views/'+ '**/*.*', './config.js', './index.html'],
		base: '.',
		dest: projectRootFolder,
		taskPath: "./gulptasks/copyFiles"
	},
	copyAssemblies: {
		src: ['../content/**/*.*'],
		base: '.',
		dest: solutionAssembliesFolder,
		taskPath: "./gulptasks/copyFiles"
	},
	concatJs: {
		src: jsFiles,
		finalName: "app.js",
		dest: projectFolderForExtensions,
		taskPath: "./gulptasks/concatJs"
	}
};

module.exports = sourceForTasks;
