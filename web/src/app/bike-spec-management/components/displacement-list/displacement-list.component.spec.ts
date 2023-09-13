import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplacementListComponent } from './displacement-list.component';

describe('DisplacementListComponent', () => {
  let component: DisplacementListComponent;
  let fixture: ComponentFixture<DisplacementListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DisplacementListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DisplacementListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
