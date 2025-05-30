# Sử dụng image localstack chính thức làm base
FROM localstack/localstack:3.8.1

# Thiết lập thư mục làm việc
WORKDIR /etc/localstack/init/ready.d

# Sao chép thư mục chứa init scripts từ host vào container
COPY ./init-aws.sh .

# Cấp quyền thực thi cho tất cả các file .sh trong thư mục init/ready.d
RUN chmod +x *.sh

# Đảm bảo entrypoint mặc định của LocalStack được giữ nguyên
# (LocalStack sẽ tự động chạy các script trong /etc/localstack/init/ready.d/)