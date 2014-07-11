



        function HomeVM() {
            var self = this;
            self.email = ko.observable();
            self.registered = ko.observable();
            self.isLoading = ko.observable(false)
            self.issues = ko.observableArray();
            self.btnInviteMeClicked = function () {
                self.isLoading(true);

                if (self.email() && self.validateEmail(self.email())) {
                    var url = '/Home/InviteMe';
                    var data = { email: self.email() };


                    $.ajax(url,
                        {
                            type: 'POST',
                            data: JSON.stringify(data),
                            contentType: "application/json",
                            dataType: 'json',
                            error: function (data) {
                                ns_MOG.displayModal("error", 'oops, something bad happened');
                                console.log(data);
                            },
                            success: function (data) {
                                if (data) {
                                    self.registered(true);
                                }
                            }
                        }
                   );


                }
                self.isLoading(false);
            }
            self.validateEmail = function (email) {

                var filter = /^([a-zA-Z0-9_\.\-])+\@@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

                return filter.test(email);
            }

            self.getGithubIssues = function () {
                var url = "https://api.github.com/repos/jrocket/mog/issues";
                $.get(url, undefined, function (data) {
                    self.issues(data);
                    console.dir(data);
                });
            }
            self.getGithubIssues();
        }
$(window).load(function () {

    $('.flexslider').flexslider({
        animation: "slide",
        slideshow: true,
        start: function (slider) {
            $('body').removeClass('loading');
        }
    });

});
var homeVM = new HomeVM();
ko.applyBindings(new HomeVM());

