from PIL import Image
import csv


def create_test_set(image_name):
    with open("/root/datascience/working_directory/test_image_csv/" + image_name.split(".")[0] + ".csv", mode='w', newline='') as image_matrix:
        matrix_writer = csv.writer(image_matrix, delimiter=',')  # Create a new CSV writer object
        print("Creating a test CSV for image " + image_name)

        # Load the test image into the pix variable using the Image function from the Pillow library
        regular_image = Image.open("/root/datascience/original/test/" + image_name)
        pix = regular_image.load()
        regular_image_width, regular_image_height = regular_image.size

        # We want to resize the image dimensions so that the width and height are divisible by 7
        if (regular_image_width % 7) == 0:
            pass
        else:
            regular_image_width = 7 * (regular_image_width // 7)

        if (regular_image_height % 7) == 0:
            pass
        else:
            regular_image_height = 7 * (regular_image_height // 7)
        start_x = 0
        while start_x < regular_image_width - 7:  # Iterate over every pixel on the x axis
            start_y = 0
            while start_y < regular_image_height - 7:  # Iterate over every pixel on the y axis
                seven_by_seven_grid = []  # Create an empty grid so that we can create a 7x7 grid
                for pixel_x in range(start_x, start_x + 7):  # Create a grid that is 7 pixels long on the x axis
                    for pixel_y in range(start_y, start_y + 7):  # Create a grid that is 7 pixels long on the y axis
                        for rgb_value in pix[pixel_x, pixel_y]:  # For every pixel in the 7x7 grid extract the RGB value
                            seven_by_seven_grid.append(rgb_value)
                seven_by_seven_grid.append(str(start_x + 3) + "|" + str(start_y + 3))  # Append the location of the center pixel
                matrix_writer.writerow(seven_by_seven_grid)
                start_y += 1
            start_x += 1
    image_matrix.close()  # Close the CSV file
    print("Created a test set CSV for the " + image_name + " image!")
