import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { FileStatus, UploadService } from '../Upload.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'BigOUploadApp';
  uploadProgress: Observable<FileStatus[]> | undefined;
  filenames: string[] = [];
  
  constructor(private uploadService: UploadService){
  }

  ngOnInit(){
    this.uploadProgress = this.uploadService.uploadProgress;
  }

  upload(files: FileList|any) {
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < files.length; i++) {
      this.filenames.push(files[i].name);
      console.log(`Uploading ${files[i].name} with size ${files[i].size} and type ${files[i].type}`);
      this.uploadService.uploadFile(files[i], files[i].name);
    }
  }
}
