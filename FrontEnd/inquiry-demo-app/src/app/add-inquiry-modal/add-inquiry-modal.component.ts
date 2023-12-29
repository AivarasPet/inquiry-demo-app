import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

interface InquiryDTO {
  message: string;
  inquiryType: number;
}

@Component({
  selector: 'app-add-inquiry-modal',
  templateUrl: './add-inquiry-modal.component.html',
  styleUrls: ['./add-inquiry-modal.component.css']
})
export class AddInquiryModalComponent implements OnInit {

  @Output() requestSuccess = new EventEmitter<any>(); 
  inquiryDto: InquiryDTO = { message: '', inquiryType: 0 };
  inquiryTypes: any;  
  
  constructor(private http: HttpClient, 
    private dialogRef: MatDialogRef<AddInquiryModalComponent>
    ) {}

  ngOnInit(): void {
    this.loadEnums();
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
