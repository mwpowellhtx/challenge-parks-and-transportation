#include "streamutil.h"
#include "splitterutil.h"

namespace challenges {

    std::vector<std::string> read_lines(std::istream & is, int count) {

        std::vector<std::string> result;

        while (count-- > 0) {
            std::string line;
            std::getline(is, line);
            result.push_back(line);
        }

        return result;
    }

    std::vector<int> read_ints(std::istream & is) {
        std::string line;
        std::getline(is, line);
        return parse_ints(line);
    }
}
