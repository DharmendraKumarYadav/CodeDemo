import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BookedBikesComponent } from './booked-bikes.component';

describe('BookedBikesComponent', () => {
  let component: BookedBikesComponent;
  let fixture: ComponentFixture<BookedBikesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookedBikesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookedBikesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
