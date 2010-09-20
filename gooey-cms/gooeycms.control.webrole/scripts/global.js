$(function(){
	$('img.rollover, .rollover img').rollover();
	$('.clear-on-focus').clearOnFocus();
});

jQuery.fn.rollover = function() {
	return this.each(function(){
		var jThis = $(this);
		if( !jThis.hasClass('active') ) {
			var sSrc = jThis.attr('src');
			var sExt = sSrc.match(/\.[a-z]{3,4}$/);
			var sNewSrc = sSrc.replace(sExt, '_on' + sExt);
			var newImg = new Image();
			newImg.src = sNewSrc;
			jThis.hover(
				function(){
					jThis.attr('src', sNewSrc);
				},
				function(){
					jThis.attr('src', sSrc);
				}
			);
		}
	});
};

jQuery.fn.clearOnFocus = function() {
	return this.each(function(){
		var jThis = $(this);
		jThis.focus(
			function(){
				jThis.addClass('focused');
				if( jThis.val() == jThis.attr('defaultValue') ) {
					jThis.val("");
				}
			}
		).blur(
			function(){
				if( jThis.val() == "" ) {
					jThis.removeClass('focused');
					jThis.val( jThis.attr('defaultValue') );
				}
			}
		)
	});
};
