equalheight = function (container) {

    var currentTallest = 0,
         currentRowStart = 0,
         rowDivs = new Array(),
         $el,
         topPosition = 0;
    $(container).each(function () {

        $el = $(this);
        $($el).height('auto')
        topPostion = $el.position().top;

        if (currentRowStart != topPostion) {
            for (currentDiv = 0 ; currentDiv < rowDivs.length ; currentDiv++) {
                rowDivs[currentDiv].height(currentTallest);
            }
            rowDivs.length = 0; // empty the array
            currentRowStart = topPostion;
            currentTallest = $el.height();
            rowDivs.push($el);
        } else {
            rowDivs.push($el);
            currentTallest = (currentTallest < $el.height()) ? ($el.height()) : (currentTallest);
        }
        for (currentDiv = 0 ; currentDiv < rowDivs.length ; currentDiv++) {
            rowDivs[currentDiv].height(currentTallest);
        }
    });
}




ko.bindingHandlers.scroll = {

    updating: true,

    init: function (element, valueAccessor, allBindingsAccessor) {
        var self = this
        self.updating = true;
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(window).off("scroll.ko.scrollHandler")
            self.updating = false
        });
    },

    update: function (element, valueAccessor, allBindingsAccessor) {
        var props = allBindingsAccessor().scrollOptions
        var offset = props.offset ? props.offset : "0"
        var loadFunc = props.loadFunc
        var load = ko.utils.unwrapObservable(valueAccessor());
        var self = this;

        if (load) {
            element.style.display = "";
            $(window).on("scroll.ko.scrollHandler", function () {
                if (($(document).height() - offset <= $(window).height() + $(window).scrollTop())) {
                    if (self.updating) {
                        loadFunc()
                        self.updating = false;
                    }
                }
                else {
                    self.updating = true;
                }
            });
        }
        else {
            element.style.display = "none";
            $(window).off("scroll.ko.scrollHandler")
            self.updating = false
        }
    }
}


function ProjectListVM(options) {

    var self = this;
    self.Page = 1;
    self.PageSize = 10;
    this.collection = ko.observableArray([])


    this.addSome = function () {

        $.post(options.getDataUrl,
            {
                PageNumber: self.Page,
                PageSize: self.PageSize
            },
            function (data) {

                for (var i = 0; i < data.length; i++) {
                    self.collection.push(data[i]);
                }
                if (!data || data.length < self.PageSize) {
                   
                    $("#loading").hide();
                }
                equalheight('.thumbnail');
               

            }
            );

        self.Page++;

    }

    this.addSome()

    self.btnLikedClicked = function (id, event) {

        var url = options.urlLike;
        $.post(url, { id: id }, function (data) {
            if (data && data.result === true) {
                var items = self.collection();
                var olditem = null;
                for (var i = 0; i < items.length; i++) {
                    if (items[i] && items[i].Id === id) {
                        olditem = items[i];
                    }
                }
                var newitem = {
                    Likes: olditem.Likes + 1,
                    Id: olditem.Id,
                    ImageUrl: olditem.ImageUrl,
                    Description: olditem.Description,
                    Name: olditem.Name,
                    IsPrivate: olditem.IsPrivate,
                    ProjectUrl: olditem.ProjectUrl
                }

                self.collection.replace(olditem, newitem);
                $("#loading").hide();
                

            }
            else {
                alert("you already liked it");
            }
        });
    }
}
