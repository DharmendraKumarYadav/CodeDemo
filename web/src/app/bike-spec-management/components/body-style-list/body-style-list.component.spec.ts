import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BodyStyleListComponent } from './body-style-list.component';

describe('BodyStyleListComponent', () => {
  let component: BodyStyleListComponent;
  let fixture: ComponentFixture<BodyStyleListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BodyStyleListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BodyStyleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
