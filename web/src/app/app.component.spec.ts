import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import {WindowProvider} from "./shared/providers";
import {InventoryService} from "./shared/inventory.service";

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        AppComponent
      ],
		providers: [WindowProvider, {
			provide: InventoryService,
			useValue: jasmine.createSpyObj('InventoryService', ['getInventory', 'uploadInventory', 'ageInventory', 'deleteTrash'])
		}]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'gilded-rose'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('gilded-rose');
  });

});
