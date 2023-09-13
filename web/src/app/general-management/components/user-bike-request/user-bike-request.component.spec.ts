import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBikeRequestComponent } from './user-bike-request.component';

describe('UserBikeRequestComponent', () => {
  let component: UserBikeRequestComponent;
  let fixture: ComponentFixture<UserBikeRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBikeRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBikeRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
