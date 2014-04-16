
function ProjectDetailVM(model)
{
    var self = this;
    self.likeCount = ko.observable(model.likes);
    self.id = model.id;
    self.urlLike = model.urlLike;

    /*
    self.urlFollow = model.urlFollow;
    self.urlUnFollow = model.urlUnFollow;
    self.isFollowed = ko.observable(model.isFollowed === 'True');

    self.btnFollowClicked = function () {
       
        $.post(self.urlFollow, { id: self.id }, function (data) {

            if (data ) {
              
                alert('you follow this project :)');
                self.isFollowed(true);
            }
        });

    }

    self.btnUnFollowClicked = function () {
       
        $.post(self.urlUnFollow, { id: self.id }, function (data) {

            if (data) {

                alert('you unfollow this project :)');
                self.isFollowed(false);
            }
        });

    }
    */
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
                alert("you already liked it");
               
            }
        });
    };

}