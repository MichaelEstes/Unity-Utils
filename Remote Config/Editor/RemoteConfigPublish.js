const fs = require('fs');
const admin = require('firebase-admin');
const configParamsJson = require('./RemoteConfigParams.json');

var configTemplate = {
    parameters: {}
};

var serviceAccount = require("Firebase Json Location");
admin.initializeApp({
    credential: admin.credential.cert(serviceAccount)
});

function getTemplate() {
    console.log(configParamsJson);
    const config = admin.remoteConfig();
    config.getTemplate()
        .then(template => {
            console.log('ETag from server: ' + template.etag);
            const templateStr = JSON.stringify(template);
            console.log(templateStr);
            // fs.writeFileSync('config.json', templateStr);
        })
        .catch(err => {
            console.error('Unable to get template');
            console.error(err);
        });
}

async function getAndUpdateTemplate() {
    const config = admin.remoteConfig();
    try {
        const template = await config.getTemplate();

        template.parameters = configTemplate.parameters;

        // Validate template after updating it.
        await config.validateTemplate(template);
        // Publish updated template.
        const updatedTemplate = await config.publishTemplate(template);
        console.log('Latest etag: ' + updatedTemplate.etag);
    } catch (err) {
        console.error('Unable to get and update template.');
        console.error(err);
    }
}

function parseConfigParams() {
    var configParamsToParse = configParamsJson["remoteConfigParams"];
    for (const param of configParamsToParse) {
        try {
            var valueObj = JSON.parse(param.value);
            if (valueObj && typeof valueObj === "object") {
                for (const [key, value] of Object.entries(valueObj)) {
                    if (value && typeof value === "object") {
                        if ('instanceID' in value) {
                            delete valueObj[key];
                        }
                    }
                }

                configTemplate.parameters[param.key] = {
                    defaultValue: {
                        value: JSON.stringify(valueObj)
                    }
                }
            }

        } catch (e) { }
    }
}

parseConfigParams();
getAndUpdateTemplate();