angular.module("main")
    .service("alertService", function (toastr) {
        var error = function (err) {
            if (err) {
                if (err.message) {
                    toastr.error(err.message, "An error has occurred");
                    return;
                }
                toastr.error(JSON.stringify(err), "An error has occurred");
                return;
            }

            toastr.error("An error has occurred.");
        };

        var info = function (message, title) {
            toastr.info(message, title);
        };

        var success = function (message, title) {
            toastr.success(message, title);
        };

        return {
            error: error,
            info: info,
            success: success
        };
    });