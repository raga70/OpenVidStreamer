let appSettings: any = null;

export const loadConfig = async () => {
    // Use the environment variable to determine the file name
    const configFileName = import.meta.env.VITE_APPSETTINGS_FILENAME;
    const response = await fetch(`/${configFileName}`);
    appSettings = await response.json();
};

export const getConfig = () => {
    if (!appSettings) {
        throw new Error("Config not loaded");
    }
    return appSettings;
};


export const ApiServerBaseUrl = () =>{
    if (!appSettings) {
        throw new Error("Config not loaded");
    }
    return appSettings.ApiServerBaseUrl;
}


export const SubscriptionCost = () =>{
    
        if (!appSettings) {
            throw new Error("Config not loaded");
        }
        return appSettings.SubscriptionCost;
    }
    
export const StripePublishableKey = () =>{
    
        if (!appSettings) {
            throw new Error("Config not loaded");
        }
        return appSettings.StripePublishableKey;
    }   