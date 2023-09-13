import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BodyStyleComponent } from './body-style.component';

describe('BodyStyleComponent', () => {
  let component: BodyStyleComponent;
  let fixture: ComponentFixture<BodyStyleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BodyStyleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BodyStyleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
