function SearchVM(options)
{
    var self = this;
    self.qPeople = ko.observable();
    self.qProject = ko.observable();
    self.results = ko.observableArray();
    self.resultCount = ko.observable(0)
    self.isLoading = ko.observable(false);

    self.btnSearchPeopleClicked = function () {
        search(options.urlSearchPeople,self.qPeople());
    }

    self.btnSearchProjectClicked = function () {
        search(options.urlSearchProject, self.qProject());
    }

    var search = function (url, query) {
        console.log(query);
        self.resultCount(0);
        self.isLoading(true);
        $.post(url,
                 {
                     query: query
                 },
                 function (data) {
                     if (data)
                     {
                         self.results(data);
                         self.resultCount(data.length);
                     }
                     else
                     {
                         self.resultCount(0);
                     }
                     
                     self.isLoading(false);
                 }
       );
    }
    

}


var bindKeyPressEnter = function(model)
{
    var btnIds = ['people', 'project'];
    for (var i = 0; i < 2; i++) {
        $('#' + btnIds[i]).keypress(function (e) {
            if (e.which == 13) {
                var btnId = $(this)[0].id;
                if (btnId === 'people')
                {
                    console.log('#btn' + btnId);
                    model.btnSearchPeopleClicked();
                }
                else
                {
                    model.btnSearchProjectClicked();
                }
               
                e.preventDefault();
            }
        });

    }

}

