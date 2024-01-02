import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddInquiryModalComponent } from './add-inquiry-modal.component';

describe('AddInquiryModalComponent', () => {
  let component: AddInquiryModalComponent;
  let fixture: ComponentFixture<AddInquiryModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddInquiryModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddInquiryModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
