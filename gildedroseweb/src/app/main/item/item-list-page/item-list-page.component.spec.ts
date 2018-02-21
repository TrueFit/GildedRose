import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemListPageComponent } from './item-list-page.component';

describe('ItemListPageComponent', () => {
  let component: ItemListPageComponent;
  let fixture: ComponentFixture<ItemListPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItemListPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
