import {Provider} from "@angular/core";

export const WindowProvider: Provider = {
	provide: 'Window',
	useValue: window,
};
