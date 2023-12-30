import { Component, NgZone, OnInit } from '@angular/core';
import { SignalRService } from '../signalr.service';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { AddInquiryModalComponent } from '../add-inquiry-modal/add-inquiry-modal.component';
import { forkJoin } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  inquiryTypes : any;
  inquiryStatuses : any;

  constructor(private http: HttpClient,
    private signalRService: SignalRService,
    public dialog: MatDialog,
    private zone: NgZone
    ) { }

  displayedColumns: string[] = ['id', 'date', 'inquiryType', 'status'];
  dataSource : any;

  ngOnInit() {
    this.setupDataListener();
    this.loadEnums();
  }
  

  private loadEnums() {
    forkJoin({
      types: this.http.get(environment.INQUIRIES_URL + '/inquiry-types'),
      statuses: this.http.get(environment.INQUIRIES_URL + '/inquiry-statuses')
    })
    .subscribe(({types, statuses}) => {
      this.inquiryTypes = types;
      this.inquiryStatuses = statuses;
      this.loadInquiries();  // Call to load inquiries after enums are loaded
    }, error => {
      console.error('Error fetching enum data', error);
    });
  }
  
  private loadInquiries() {
    this.http.get(environment.INQUIRIES_URL).subscribe(response => {
      this.dataSource = response;
    })
  }

  getTypeLabel(typeId: number): string {
    const type = this.inquiryTypes?.find((t : any) => t.value === typeId);
    return type ? type.label : '';
  }
  
  getStatusLabel(statusId: number): string {
    const status = this.inquiryStatuses?.find((s: any) => s.value === statusId);
    return status ? status.label : '';
  }  
  
  private setupDataListener(): void {
    this.signalRService.startConnection();
    this.signalRService.addTransferDataListener(environment.INQUIRIES_REFRESH_TOPIC, (data) => {
      this.zone.run(() => {
        this.loadInquiries();
      });
    });
  }

  openAddInquiryModal(): void {
    const dialogRef = this.dialog.open(AddInquiryModalComponent);
    const sub = dialogRef.componentInstance.requestSuccess.subscribe((response) => {
      this.loadInquiries();
      sub.unsubscribe(); // Unsubscribe to avoid memory leaks
    });
  }
}
