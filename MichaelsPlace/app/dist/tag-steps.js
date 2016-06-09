'use strict';

System.register(['aurelia-framework', 'aurelia-fetch-client', 'fetch'], function (_export, _context) {
  "use strict";

  var inject, HttpClient, _dec, _class, TagSteps;

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  return {
    setters: [function (_aureliaFramework) {
      inject = _aureliaFramework.inject;
    }, function (_aureliaFetchClient) {
      HttpClient = _aureliaFetchClient.HttpClient;
    }, function (_fetch) {}],
    execute: function () {
      _export('TagSteps', TagSteps = (_dec = inject(HttpClient), _dec(_class = function () {
        function TagSteps(http) {
          _classCallCheck(this, TagSteps);

          this.heading = 'Describe Yourself';
          this.tags = [];

          this.http = http;
        }

        TagSteps.prototype.activate = function activate() {
          var _this = this;

          return this.http.fetch('tags/demographic').then(function (response) {
            return response.json();
          }).then(function (tags) {
            return _this.tags = tags;
          });
        };

        return TagSteps;
      }()) || _class));

      _export('TagSteps', TagSteps);
    }
  };
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInRhZy1zdGVwcy5qcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7Ozs7Ozs7Ozs7QUFBUSxZLHFCQUFBLE07O0FBQ0EsZ0IsdUJBQUEsVTs7OzBCQUlLLFEsV0FEWixPQUFPLFVBQVAsQztBQUtDLDBCQUFZLElBQVosRUFBa0I7QUFBQTs7QUFBQSxlQUhsQixPQUdrQixHQUhSLG1CQUdRO0FBQUEsZUFGbEIsSUFFa0IsR0FGWCxFQUVXOztBQUNoQixlQUFLLElBQUwsR0FBWSxJQUFaO0FBQ0Q7OzJCQUVELFEsdUJBQVc7QUFBQTs7QUFDVCxpQkFBTyxLQUFLLElBQUwsQ0FBVSxLQUFWLENBQWdCLGtCQUFoQixFQUNKLElBREksQ0FDQztBQUFBLG1CQUFZLFNBQVMsSUFBVCxFQUFaO0FBQUEsV0FERCxFQUVKLElBRkksQ0FFQztBQUFBLG1CQUFRLE1BQUssSUFBTCxHQUFZLElBQXBCO0FBQUEsV0FGRCxDQUFQO0FBR0QsUyIsImZpbGUiOiJ0YWctc3RlcHMuanMiLCJzb3VyY2VSb290IjoiL3NyYyJ9
