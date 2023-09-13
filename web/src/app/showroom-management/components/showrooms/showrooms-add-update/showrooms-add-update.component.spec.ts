import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowroomsAddUpdateComponent } from './showrooms-add-update.component';

describe('ShowroomsAddUpdateComponent', () => {
  let component: ShowroomsAddUpdateComponent;
  let fixture: ComponentFixture<ShowroomsAddUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowroomsAddUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowroomsAddUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
