import {Component, OnInit, OnDestroy} from '@angular/core';

import {Subscription} from 'rxjs/Subscription';

import {MediaChange, ObservableMedia} from '@angular/flex-layout';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy {
    watcher: Subscription;
    activeMq = "";
    sideNavMode;
    sideNavOpened;

    constructor(media: ObservableMedia) {
        this.watcher = media.subscribe((change: MediaChange) => {
            this.setNavigationMode(change);
        });
    }

    ngOnInit() {
        this.sideNavMode = "side";
        this.sideNavOpened = true;
    }

    ngOnDestroy() {
        this.watcher.unsubscribe();
    }

    setNavigationMode(change: MediaChange): void {
        if(change.mqAlias === 'sm'  || change.mqAlias === 'xs') {
            this.sideNavMode = 'push';
            this.sideNavOpened = false;
        } else {
            this.sideNavMode = "side";
            this.sideNavOpened = true;
        }
    }
}
