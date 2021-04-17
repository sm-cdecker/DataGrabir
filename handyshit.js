$("div.freebirdAnalyticsViewAnalyticsItem").toArray().map(elem => {
	return {
		'name': $(elem).find('span').html(),
		'fieldId': 'entry.' + $(elem).find('div[data-fieldid]').attr('data-fieldid')
	}
})