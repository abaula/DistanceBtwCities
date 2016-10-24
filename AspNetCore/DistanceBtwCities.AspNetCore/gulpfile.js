/// <binding BeforeBuild="clean:wwwroot, build:increaseBuildNumber, build:html, build:js, build:css, copy:jslibs, copy:csslibs, copy:fonts" Clean="clean:wwwroot" />
"use strict";

var fs = require("fs");
var path = require("path");
var dir = require("node-dir");
var gulp = require("gulp");
var concat = require("gulp-concat");
var uglify = require("gulp-uglify");
var del = require("del");
var header = require("gulp-header");
var less = require("gulp-less");
var minifycss = require("gulp-minify-css");
var replace = require("gulp-replace");

var settings = {
    appName: "DistanceBetweenCities",
    appVersion: "1.0.0",
    url: "https://github.com/abaula/DistanceBtwCities",
    copyright: "Copyright 2016 / Anton Baula, anton.baula@gmail.com",
    path: {
        build: {
            root: "wwwroot",
            html: "wwwroot",
            js: "wwwroot/js",
            style: "wwwroot/css",
            font: "wwwroot/fonts"
        },
        src: {
            html: "ClientApp/*.html",
            js: ["ClientApp/js/app.js",
                "ClientApp/js/headController.js",
                "ClientApp/js/searchPanelController.js",
                "ClientApp/js/searchResultsController.js"
            ],
            style: "ClientApp/css/*.less",
            font: "bower_components/bootstrap/fonts/*",
            buildNumber: "BuildNumber.txt",
            jslibs: [
                "bower_components/angular/angular.min.js",
                "bower_components/angular-animate/angular-animate.min.js",
                "bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js",
                "bower_components/angular-route/angular-route.min.js"
            ],
            csslibs: [
                "bower_components/bootstrap/dist/css/bootstrap.min.css",
                "bower_components/bootstrap/dist/css/bootstrap-theme.min.css"
            ]
        }
    }
};


function getAppVersion()
{
    var buildNumber = fs.readFileSync(settings.path.src.buildNumber);
    return settings.appVersion + "." + buildNumber;
}

gulp.task("build:increaseBuildNumber", function ()
{
    var buildNumber = parseInt(fs.readFileSync(settings.path.src.buildNumber));
    fs.writeFileSync(settings.path.src.buildNumber, ++buildNumber);
});

gulp.task("clean:wwwroot", function ()
{
    return del(settings.path.build.root + "/**/*");
});

gulp.task("build:html", function ()
{
    var appVersion = getAppVersion();

    return gulp.src(settings.path.src.html)
        .pipe(header(fs.readFileSync("Copyrights/Html.txt"),
            {
                version: appVersion,
                appName: settings.appName,
                url: settings.url,
                copyright: settings.copyright
            }))
        .pipe(replace("{#version#}", appVersion))
        .pipe(gulp.dest(settings.path.build.html));
});

gulp.task("build:js", function ()
{
    var appVersion = getAppVersion();

    return gulp.src(settings.path.src.js)
        //.pipe(uglify())
        .pipe(concat("app.min." + appVersion + ".js"))
        .pipe(header(fs.readFileSync("Copyrights/Js.txt"),
        {
            version: appVersion,
            appName: settings.appName,
            url: settings.url,
            copyright: settings.copyright
        }))
        .pipe(gulp.dest(settings.path.build.js));
});

gulp.task("build:css", function ()
{
    var appVersion = getAppVersion();

    return gulp.src(settings.path.src.style)
        .pipe(less())
        //.pipe(minifycss())
        .pipe(concat("app.min." + appVersion + ".css"))
        .pipe(header(fs.readFileSync("Copyrights/Js.txt"),
        {
            version: appVersion,
            appName: settings.appName,
            url: settings.url,
            copyright: settings.copyright
        }))
        .pipe(gulp.dest(settings.path.build.style));
});

gulp.task("copy:jslibs", function ()
{
    return gulp.src(settings.path.src.jslibs)
        .pipe(gulp.dest(settings.path.build.js));
});

gulp.task("copy:csslibs", function ()
{
    return gulp.src(settings.path.src.csslibs)
        .pipe(gulp.dest(settings.path.build.style));
});

gulp.task("copy:fonts", function ()
{
    return gulp.src(settings.path.src.font)
        .pipe(gulp.dest(settings.path.build.font));
});