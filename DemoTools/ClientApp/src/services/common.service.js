import { __decorate } from "tslib";
import { injectable } from "inversify";
import { useToast } from 'vue-toastification';
let CommonService = class CommonService {
    constructor() {
        this.toast = useToast();
    }
    webNotify(message, type) {
        if (type && (type.toLowerCase() == 'success' || type.toLowerCase() == 'ok'))
            this.toast.success(message);
        else if (type && (type.toLowerCase() == 'error' || type.toLowerCase() == 'no'))
            this.toast.error(message);
        else
            this.toast.info(message);
    }
    webNotifyOk(message) {
        this.toast.success(message);
    }
    webNotifyError(message) {
        this.toast.error(message);
    }
    webNotifyInfo(message) {
        this.toast.info(message);
    }
    webNotifyException(err) {
        var message = err && err.response && err.response.data ? err.response.data : "Unexpected error accured!";
        this.toast.error(message);
    }
};
CommonService = __decorate([
    injectable()
], CommonService);
export { CommonService };
//# sourceMappingURL=common.service.js.map