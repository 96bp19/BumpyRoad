//
//  AdjustUnity.mm
//  Adjust SDK
//
//  Created by Pedro Silva (@nonelse) on 27th March 2014.
//  Copyright © 2012-2018 Adjust GmbH. All rights reserved.
//

#import "Adjust.h"
#import "ADJEvent.h"
#import "ADJConfig.h"
#import "AdjustUnity.h"
#import "AdjustUnityDelegate.h"

@implementation AdjustUnity

#pragma mark - Object lifecycle methods

- (id)init {
    self = [super init];
    if (nil == self) {
        return nil;
    }
    return self;
}

@end

#pragma mark - Helper C methods

// Method for converting JSON stirng parameters into NSArray object.
NSArray* convertArrayParameters(const char* cStringJsonArrayParameters) {
    if (cStringJsonArrayParameters == NULL) {
        return nil;
    }

    NSError *error = nil;
    NSArray *arrayParameters = nil;
    NSString *stringJsonArrayParameters = [NSString stringWithUTF8String:cStringJsonArrayParameters];

    if (stringJsonArrayParameters != nil) {
        NSData *dataJson = [stringJsonArrayParameters dataUsingEncoding:NSUTF8StringEncoding];
        arrayParameters = [NSJSONSerialization JSONObjectWithData:dataJson options:0 error:&error];
    }
    if (error != nil) {
        NSString *errorMessage = @"Failed to parse json parameters!";
        NSLog(@"%@", errorMessage);
    }

    return arrayParameters;
}

BOOL isStringValid(const char* cString) {
    if (cString == NULL) {
        return false;
    }

    NSString *objcString = [NSString stringWithUTF8String:cString];
    if (objcString == nil) {
        return false;
    }
    if ([objcString isEqualToString:@"ADJ_INVALID"]) {
        return false;
    }

    return true;
}

void addValueOrEmpty(NSMutableDictionary *dictionary, NSString *key, NSObject *value) {
    if (nil != value) {
        [dictionary setObject:[NSString stringWithFormat:@"%@", value] forKey:key];
    } else {
        [dictionary setObject:@"" forKey:key];
    }
}

#pragma mark - Publicly available C methods

extern "C"
{
    void _AdjustLaunchApp(const char* appToken,
                          const char* environment,
                          const char* sdkPrefix,
                          const char* userAgent,
                          const char* defaultTracker,
                          const char* sceneName,
                          int allowSuppressLogLevel,
                          int logLevel,
                          int isDeviceKnown,
                          int eventBuffering,
                          int sendInBackground,
                          int64_t secretId,
                          int64_t info1,
                          int64_t info2,
                          int64_t info3,
                          int64_t info4,
                          double delayStart,
                          int launchDeferredDeeplink,
                          int isAttributionCallbackImplemented,
                          int isEventSuccessCallbackImplemented,
                          int isEventFailureCallbackImplemented,
                          int isSessionSuccessCallbackImplemented,
                          int isSessionFailureCallbackImplemented,
                          int isDeferredDeeplinkCallbackImplemented) {
        NSString *stringAppToken = isStringValid(appToken) == true ? [NSString stringWithUTF8String:appToken] : nil;
        NSString *stringEnvironment = isStringValid(environment) == true ? [NSString stringWithUTF8String:environment] : nil;
        NSString *stringSdkPrefix = isStringValid(sdkPrefix) == true ? [NSString stringWithUTF8String:sdkPrefix] : nil;
        NSString *stringUserAgent = isStringValid(userAgent) == true ? [NSString stringWithUTF8String:userAgent] : nil;
        NSString *stringDefaultTracker = isStringValid(defaultTracker) == true ? [NSString stringWithUTF8String:defaultTracker] : nil;
        NSString *stringSceneName = isStringValid(sceneName) == true ? [NSString stringWithUTF8String:sceneName] : nil;

        ADJConfig *adjustConfig;

        if (allowSuppressLogLevel != -1) {
            adjustConfig = [ADJConfig configWithAppToken:stringAppToken
                                             environment:stringEnvironment
                                   allowSuppressLogLevel:(BOOL)allowSuppressLogLevel];
        } else {
            adjustConfig = [ADJConfig configWithAppToken:stringAppToken
                                             environment:stringEnvironment];
        }

        // Set SDK prefix.
        [adjustConfig setSdkPrefix:stringSdkPrefix];

        // Check if user has selected to implement any of the callbacks.
        if (isAttributionCallbackImplemented
            || isEventSuccessCallbackImplemented
            || isEventFailureCallbackImplemented
            || isSessionSuccessCallbackImplemented
            || isSessionFailureCallbackImplemented
            || isDeferredDeeplinkCallbackImplemented) {
            [adjustConfig setDelegate:
                [AdjustUnityDelegate getInstanceWithSwizzleOfAttributionCallback:isAttributionCallbackImplemented
                                                            eventSuccessCallback:isEventSuccessCallbackImplemented
                                                            eventFailureCallback:isEventFailureCallbackImplemented
                                                          sessionSuccessCallback:isSessionSuccessCallbackImplemented
                                                          sessionFailureCallback:isSessionFailureCallbackImplemented
                                                        deferredDeeplinkCallback:isDeferredDeeplinkCallbackImplemented
                                                    shouldLaunchDeferredDeeplink:launchDeferredDeeplink
                                                        withAdjustUnitySceneName:stringSceneName]];
        }

        // Log level.
        if (logLevel != -1) {
            [adjustConfig setLogLevel:(ADJLogLevel)logLevel];
        }

        // Event buffering.
        if (eventBuffering != -1) {
            [adjustConfig setEventBufferingEnabled:(BOOL)eventBuffering];
        }

        // Send in background.
        if (sendInBackground != -1) {
            [adjustConfig setSendInBackground:(BOOL)sendInBackground];
        }

        // Device known.
        if (isDeviceKnown != -1) {
            [adjustConfig setIsDeviceKnown:(BOOL)isDeviceKnown];
        }

        // Delay start.
        if (delayStart != -1) {
            [adjustConfig setDelayStart:delayStart];
        }

        // User agent.
        if (stringUserAgent != nil) {
            [adjustConfig setUserAgent:stringUserAgent];
        }

        // Default tracker.
        if (stringDefaultTracker != nil) {
            [adjustConfig setDefaultTracker:stringDefaultTracker];
        }

        // App secret.
        if (secretId != -1 && info1 != -1 && info2 != -1 && info3 != -1 && info4 != 1) {
            [adjustConfig setAppSecret:secretId info1:info1 info2:info2 info3:info3 info4:info4];
        }

        // Start the SDK.
        [Adjust appDidLaunch:adjustConfig];
        [Adjust trackSubsessionStart];
    }

    void _AdjustTrackEvent(const char* eventToken,
                           double revenue,
                           const char* currency,
                           const char* receipt,
                           const char* transactionId,
                           const char* callbackId,
                           int isReceiptSet,
                           const char* jsonCallbackParameters,
                           const char* jsonPartnerParameters) {
        NSString *stringEventToken = isStringValid(eventToken) == true ? [NSString stringWithUTF8String:eventToken] : nil;
        ADJEvent *event = [ADJEvent eventWithEventToken:stringEventToken];

        // Revenue and currency.
        if (revenue != -1 && currency != NULL) {
            NSString *stringCurrency = [NSString stringWithUTF8String:currency];
            [event setRevenue:revenue currency:stringCurrency];
        }

        // Callback parameters.
        NSArray *arrayCallbackParameters = convertArrayParameters(jsonCallbackParameters);
        if (arrayCallbackParameters != nil) {
            NSUInteger count = [arrayCallbackParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayCallbackParameters[i++];
                NSString *value = arrayCallbackParameters[i++];
                [event addCallbackParameter:key value:value];
            }
        }

        NSArray *arrayPartnerParameters = convertArrayParameters(jsonPartnerParameters);
        if (arrayPartnerParameters != nil) {
            NSUInteger count = [arrayPartnerParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayPartnerParameters[i++];
                NSString *value = arrayPartnerParameters[i++];
                [event addPartnerParameter:key value:value];
            }
        }

        // Transaction ID.
        if (transactionId != NULL) {
            NSString *stringTransactionId = [NSString stringWithUTF8String:transactionId];
            [event setTransactionId:stringTransactionId];
        }

        // Callback ID.
        if (callbackId != NULL) {
            NSString *stringCallbackId = [NSString stringWithUTF8String:callbackId];
            [event setCallbackId:stringCallbackId];
        }

        // Receipt (legacy).
        if ([[NSNumber numberWithInt:isReceiptSet] boolValue]) {
            NSString *stringReceipt = nil;
            NSString *stringTransactionId = nil;

            if (receipt != NULL) {
                stringReceipt = [NSString stringWithUTF8String:receipt];
            }
            if (transactionId != NULL) {
                stringTransactionId = [NSString stringWithUTF8String:transactionId];
            }

            [event setReceipt:[stringReceipt dataUsingEncoding:NSUTF8StringEncoding] transactionId:stringTransactionId];
        }

        // Track event.
        [Adjust trackEvent:event];
    }

    void _AdjustTrackSubsessionStart() {
        [Adjust trackSubsessionStart];
    }

    void _AdjustTrackSubsessionEnd() {
        [Adjust trackSubsessionEnd];
    }

    void _AdjustSetEnabled(int enabled) {
        BOOL bEnabled = (BOOL)enabled;
        [Adjust setEnabled:bEnabled];
    }

    int _AdjustIsEnabled() {
        BOOL isEnabled = [Adjust isEnabled];
        int iIsEnabled = (int)isEnabled;
        return iIsEnabled;
    }

    void _AdjustSetOfflineMode(int enabled) {
        BOOL bEnabled = (BOOL)enabled;
        [Adjust setOfflineMode:bEnabled];
    }

    void _AdjustSetDeviceToken(const char* deviceToken) {
        NSString *stringDeviceToken = [NSString stringWithUTF8String:deviceToken];
        [Adjust setPushToken:stringDeviceToken];
    }

    void _AdjustAppWillOpenUrl(const char* url) {
        NSString *stringUrl = [NSString stringWithUTF8String:url];
        NSURL *nsUrl;
        if ([NSString instancesRespondToSelector:@selector(stringByAddingPercentEncodingWithAllowedCharacters:)]) {
            nsUrl = [NSURL URLWithString:[stringUrl stringByAddingPercentEncodingWithAllowedCharacters:[NSCharacterSet URLFragmentAllowedCharacterSet]]];
        } else {
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
            nsUrl = [NSURL URLWithString:[stringUrl stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
        }
#pragma clang diagnostic pop

        [Adjust appWillOpenUrl:nsUrl];
    }

    char* _AdjustGetIdfa() {
        NSString *idfa = [Adjust idfa];
        if (nil == idfa) {
            return NULL;
        }

        const char* idfaCString = [idfa UTF8String];
        if (NULL == idfaCString) {
            return NULL;
        }

        char* idfaCStringCopy = strdup(idfaCString);
        return idfaCStringCopy;
    }

    char* _AdjustGetAdid() {
        NSString *adid = [Adjust adid];
        if (nil == adid) {
            return NULL;
        }

        const char* adidCString = [adid UTF8String];
        if (NULL == adidCString) {
            return NULL;
        }

        char* adidCStringCopy = strdup(adidCString);
        return adidCStringCopy;
    }

    char* _AdjustGetSdkVersion() {
        NSString *sdkVersion = [Adjust sdkVersion];
        if (nil == sdkVersion) {
            return NULL;
        }

        const char* sdkVersionCString = [sdkVersion UTF8String];
        if (NULL == sdkVersionCString) {
            return NULL;
        }

        char* sdkVersionCStringCopy = strdup(sdkVersionCString);
        return sdkVersionCStringCopy;
    }

    char* _AdjustGetAttribution() {
        ADJAttribution *attribution = [Adjust attribution];
        if (nil == attribution) {
            return NULL;
        }

        NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
        addValueOrEmpty(dictionary, @"trackerToken", attribution.trackerToken);
        addValueOrEmpty(dictionary, @"trackerName", attribution.trackerName);
        addValueOrEmpty(dictionary, @"network", attribution.network);
        addValueOrEmpty(dictionary, @"campaign", attribution.campaign);
        addValueOrEmpty(dictionary, @"creative", attribution.creative);
        addValueOrEmpty(dictionary, @"adgroup", attribution.adgroup);
        addValueOrEmpty(dictionary, @"clickLabel", attribution.clickLabel);
        addValueOrEmpty(dictionary, @"adid", attribution.adid);

        NSData *dataAttribution = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:nil];
        NSString *stringAttribution = [[NSString alloc] initWithBytes:[dataAttribution bytes]
                                                               length:[dataAttribution length]
                                                             encoding:NSUTF8StringEncoding];
        const char* attributionCString = [stringAttribution UTF8String];
        char* attributionCStringCopy = strdup(attributionCString);
        return attributionCStringCopy;
    }

    void _AdjustSendFirstPackages() {
        [Adjust sendFirstPackages];
    }

    void _AdjustGdprForgetMe() {
        [Adjust gdprForgetMe];
    }

    void _AdjustAddSessionPartnerParameter(const char* key, const char* value) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        NSString *stringValue = [NSString stringWithUTF8String:value];
        [Adjust addSessionPartnerParameter:stringKey value:stringValue];
    }

    void _AdjustAddSessionCallbackParameter(const char* key, const char* value) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        NSString *stringValue = [NSString stringWithUTF8String:value];
        [Adjust addSessionCallbackParameter:stringKey value:stringValue];
    }

    void _AdjustRemoveSessionPartnerParameter(const char* key) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        [Adjust removeSessionPartnerParameter:stringKey];
    }

    void _AdjustRemoveSessionCallbackParameter(const char* key) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        [Adjust removeSessionCallbackParameter:stringKey];
    }

    void _AdjustResetSessionPartnerParameters() {
        [Adjust resetSessionPartnerParameters];
    }

    void _AdjustResetSessionCallbackParameters() {
        [Adjust resetSessionCallbackParameters];
    }

    void _AdjustTrackAdRevenue(const char* source, const char* payload) {
        NSString *stringSource = [NSString stringWithUTF8String:source];
        NSString *stringPayload = [NSString stringWithUTF8String:payload];
        NSData *dataPayload = [stringPayload dataUsingEncoding:NSUTF8StringEncoding];
        [Adjust trackAdRevenue:stringSource payload:dataPayload];
    }

    void _AdjustSetTestOptions(const char* baseUrl,
                               const char* basePath,
                               const char* gdprUrl,
                               const char* gdprPath,
                               long timerIntervalInMilliseconds,
                               long timerStartInMilliseconds,
                               long sessionIntervalInMilliseconds,
                               long subsessionIntervalInMilliseconds,
                               int teardown,
                               int deleteState,
                               int noBackoffWait,
                               int iAdFrameworkEnabled) {
        AdjustTestOptions *testOptions = [[AdjustTestOptions alloc] init];

        NSString *stringBaseUrl = isStringValid(baseUrl) == true ? [NSString stringWithUTF8String:baseUrl] : nil;
        if (stringBaseUrl != nil) {
            [testOptions setBaseUrl:stringBaseUrl];
        }

        NSString *stringGdprUrl = isStringValid(baseUrl) == true ? [NSString stringWithUTF8String:gdprUrl] : nil;
        if (stringGdprUrl != nil) {
            [testOptions setGdprUrl:stringGdprUrl];
        }

        NSString *stringBasePath = isStringValid(basePath) == true ? [NSString stringWithUTF8String:basePath] : nil;
        if (stringBasePath != nil && [stringBasePath length] > 0) {
            [testOptions setBasePath:stringBasePath];
        }

        NSString *stringGdprPath = isStringValid(gdprPath) == true ? [NSString stringWithUTF8String:gdprPath] : nil;
        if (stringGdprPath != nil && [stringGdprPath length] > 0) {
            [testOptions setGdprPath:stringGdprPath];
        }

        testOptions.timerIntervalInMilliseconds = [NSNumber numberWithLong:timerIntervalInMilliseconds];
        testOptions.timerStartInMilliseconds = [NSNumber numberWithLong:timerStartInMilliseconds];
        testOptions.sessionIntervalInMilliseconds = [NSNumber numberWithLong:sessionIntervalInMilliseconds];
        testOptions.subsessionIntervalInMilliseconds = [NSNumber numberWithLong:subsessionIntervalInMilliseconds];

        if (teardown != -1) {
            [AdjustUnityDelegate teardown];
            [testOptions setTeardown:(BOOL)teardown];
        }
        if (deleteState != -1) {
            [testOptions setDeleteState:(BOOL)deleteState];
        }
        if (noBackoffWait != -1) {
            [testOptions setNoBackoffWait:(BOOL)noBackoffWait];
        }
        if (iAdFrameworkEnabled != -1) {
            [testOptions setIAdFrameworkEnabled:(BOOL)iAdFrameworkEnabled];
        }

        [Adjust setTestOptions:testOptions];
    }
}
