const Jasmine = require('jasmine');
const path = require('path');

module.exports = function (callback, testFile) {
    var jasmine = new Jasmine();
    jasmine.randomizeTests(false);
    jasmine.configureDefaultReporter({
        showColors: false
    });
    if (path.extname(testFile) == '.ts') {
        jasmine.helperFiles = ["ts-node/register/type-check.js"];
    }

    var out = "";
    process.stdout.write = (str, encording, callback) => {
        out += str;
        return true;
    };
    process.stderr.write = (str, encording, callback) => {
        out += str;
        return true;
    };

    jasmine.onComplete(function (passed) {
        callback(null, {
            testFile: testFile,
            passed: passed,
            output: out
        });
    });

    jasmine.execute([testFile]);
}; 