
function ProjectDetailVM(model)
{
    var self = this;
    self.likeCount = ko.observable(model.likes);
    self.id = model.id;
    self.urlLike = model.urlLike;

   
    self.btnLikeClicked = function () {

      
        $.post(self.urlLike, { id: self.id }, function (data) {
           
            if (data && data.result === true) {
                var count = parseInt(self.likeCount(), 10);
                if (count)
                {
                    count++;
                    self.likeCount(count);
                }
                else
                {
                    self.likeCount(1);
                }

            }
            else {
                ns_MOG.displayModal("Sorry", "You already liked it");
               
            }
        });
    };

}