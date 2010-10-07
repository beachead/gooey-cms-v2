$(function(){

	var jPriceList = $('<ul/>');
	for(var i=0; i<prices.length; i++){
		var jItem = $('<li><a href="#">' + prices[i] + '</a></li>');
		jPriceList.append(jItem);
	}

	$('#price-filter div.menu').append( jPriceList );

	var jThumbs = $('ul.thumbs');
	var jPager = jThumbs.parent().find('ul.thumb-nav');
	jThumbs.each(
		function() {
			var jThis = $(this);
			var jParent = jThis.parent();
			var jPager = jParent.find('ul.thumb-nav');
			var jNext = jParent.find('div.next');
			var jPrev = jParent.find('div.prev');
			jThis.cycle({
				fx: 'scrollHorz',
				speed: 500,
			    pager: jPager,
				pagerAnchorBuilder: function(index, DOMelement){
					return '<li><a href=""></a></li>';
				},
				timeout: 0,
				activePagerClass: 'active',
				pagerClick: function(zeroBasedSlideIndex, slideElement){},
				pagerEvent: 'click.cycle',
		        next: jNext,
	    	    prev: jPrev

			});

		}
	);
});