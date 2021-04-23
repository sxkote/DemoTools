import { inject, injectable } from "inversify";
import { ICommonService } from '@/interfaces/interfaces'
import { useToast } from 'vue-toastification'


@injectable()
export class CommonService implements ICommonService {
    toast = useToast();

    webNotify(message: string, type: string) {
        if (type && (type.toLowerCase() == 'success' || type.toLowerCase() == 'ok'))
            this.toast.success(message);
        else if (type && (type.toLowerCase() == 'error' || type.toLowerCase() == 'no'))
            this.toast.error(message);
        else
            this.toast.info(message);
    }

    webNotifyOk(message: string) {
        this.toast.success(message);
    }

    webNotifyError(message: string) {
        this.toast.error(message);
    }

    webNotifyInfo(message: string) {
        this.toast.info(message);
    }

    webNotifyException(err: any) {
        var message = err && err.response && err.response.data ? err.response.data : "Unexpected error accured!";
        this.toast.error(message);
    }
}
