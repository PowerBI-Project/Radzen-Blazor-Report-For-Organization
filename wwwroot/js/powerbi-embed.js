 function embedReport(containerId, reportId, embedUrl, token) {
    var reportContainer = document.getElementById(containerId)

    var config = {
        type: 'report',
        id: reportId,
        embedUrl: embedUrl,
        accessToken: token,
    
        settings: {
            filterPaneEnabled: false,
            navContentPaneEnabled: false
        }
    }

    var report = powerbi.embed(reportContainer, config)

    report.on("loaded", function () {
        report.off("loaded");
    })

    report.on("rendered", function () {
        report.off("rendered");
    })
}
