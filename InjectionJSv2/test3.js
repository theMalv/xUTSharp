var assert = require('assert');

describe('Test Suite 1', function () {
    it('Test 1', function () {
        assert.ok(true, "This shouldn't fail");
    });
});

describe('Test Suite 2', function () {
    it('Test 2', function () {
        assert.ok(false, "This should fail");
    });
});