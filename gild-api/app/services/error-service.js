angular.module("main")
    .service("errorService", function (toastr) {
        var onError = function (err) {
            if (err) {
                if (err.message) {

                    toastr.error(err.message, "An error has occurred");
                    //alert(err.message);
                    return;
                }
                //alert(JSON.stringify(err));
                toastr.error(JSON.stringify(err), "An error has occurred");
                return;
            }

            toastr.error("An error has occurred.");
        };

        return {
            onError: onError
        };
    });