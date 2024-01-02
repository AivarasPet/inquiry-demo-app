import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

interface InquiryDTO {
  message: string;
  type: number;
}

@Component({
  selector: 'app-add-inquiry-modal',
  templateUrl: './add-inquiry-modal.component.html',
  styleUrls: ['./add-inquiry-modal.component.css']
})
export class AddInquiryModalComponent implements OnInit {

  @Output() requestSuccess = new EventEmitter<any>(); 
  inquiryDto = { message: '', type: '' };
  inquiryTypes: any;  
  
  constructor(private http: HttpClient, 
    private dialogRef: MatDialogRef<AddInquiryModalComponent>
    ) {}

  ngOnInit(): void {
    this.loadEnums();
  }

  get selectedType() {
    return this.inquiryDto.type;
  }

  set selectedType(value: string) {
      this.inquiryDto.type = value;
  }

  onTypeChange(value: string) {
      this.selectedType = value;
  }

  private loadEnums() {
    this.http.get(environment.INQUIRIES_URL + '/inquiry-types').subscribe(
      (response: any) => {
        this.inquiryTypes = response;
      },
      error => {
        console.error('Error fetching inquiry types', error);
      }
    );
  }

  onConfirm() {
    this.http.post(environment.INQUIRIES_URL, this.inquiryDto).subscribe({
      next: (response) => {
        this.requestSuccess.emit(response);  
      },
      error: (error) => {
        console.error('Error submitting inquiry', error);
      }
    });
  }

}
