var count = 2;

$(function () {
    $("#AddPlayer").click(function () {
        alert("ok");
        $(this).animate({ height: '+=25', width: '+=25' })
        .animate({ height: '-=25', width: '-=25' });
        if (count < 2)
        {
            count == 2
        }
        count++;
        if (count <= 4)
        {
            $("#placeholder").append($("#template" + count.toString()).html());
            $("#TeamPlayer" + count.toString()).val(count);
            $(".badge").html(count);
        }
        
    });
});

$(function () {
    $("#War").click(function () {
        alert("OK");
        $("#AddPlayer").removeClass("hidden");
        $("#placeholder").html($("#template2").html());
        $("#TeamPlayer1").val(1);
        $("#TeamPlayer2").val(2);
        count = 2;
        $(".badge").html(count);
    });
});

$(function () {
    $("#Teams").click(function () {
        alert("OK");
        $("#AddPlayer").addClass("hidden");
        $("#placeholder").html($("#template2").html());
        $("#Team1Show").removeClass("hidden");
        $("#TeamPlayer1").val(1);
        $("#TeamPlayer2").val(1);
        $("#placeholder").append($("#template3").html());
        $("#Team2Show").removeClass("hidden");
        $("#TeamPlayer3").val(2);
        $("#placeholder").append($("#template4").html());
        $("#TeamPlayer4").val(2);
    });
});