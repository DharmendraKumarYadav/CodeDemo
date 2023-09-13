import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileUploadStatusBarComponent } from './file-upload-status-bar.component';

describe('FileUploadStatusBarComponent', () => {
  let component: FileUploadStatusBarComponent;
  let fixture: ComponentFixture<FileUploadStatusBarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileUploadStatusBarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileUploadStatusBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
